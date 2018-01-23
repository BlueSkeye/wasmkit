using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasmLib
{
    internal class ValidationContext
    {
        internal  ValidationContext(WasmModule module)
        {
            _module = module;
            Errors = new List<string>();
            Labels = new List<Tuple<BuiltinLanguageType, int, bool>>();
            _stack = new List<BuiltinLanguageType>();
            InitializeGlobalVariables();
            InitializeMemoryMap();
        }

        internal List<string> Errors { get; private set; }

        private List<Tuple<BuiltinLanguageType, int, bool>> Labels { get; set; }

        internal void AddError(string message)
        {
            Errors.Add(message);
            return;
        }

        internal void CreateLabel(BuiltinLanguageType type, bool allowReuseForElse)
        {
            Labels.Add(new Tuple<BuiltinLanguageType, int, bool>(type, (_currentStackBarrier = _stack.Count), allowReuseForElse));
        }

        internal BuiltinLanguageType GetGlobalVariableType(uint index)
        {
            if (_globalTypes.Length <= index) {
                Errors.Add(string.Format("{0} is an invalid global variable index.", index));
                return 0;
            }
            return _globalTypes[index];
        }

        internal BuiltinLanguageType GetLocalVariableType(uint index)
        {
            if (_localTypes.Length <= index) {
                Errors.Add(string.Format("{0} is an invalid local variable index.", index));
                return 0;
            }
            return _localTypes[index];
        }

        private BuiltinLanguageType GetStackItem(int stackIndex)
        {
            if (0 > stackIndex) { throw new ArgumentOutOfRangeException(); }
            if (0 > stackIndex) {
                Errors.Add(string.Format("Attempt to pop on empty stack."));
                return 0;
            }
            if (stackIndex < _currentStackBarrier) {
                Errors.Add(string.Format("Attempt to pop a stack element beyond stack barrier."));
                return 0;
            }
            return _stack[stackIndex];
        }

        private void InitializeGlobalVariables()
        {
            List<BuiltinLanguageType> globals = new List<BuiltinLanguageType>();

            foreach (ImportedGlobalDefinition import in _module.EnumerateGlobalImports()) {
                globals.Add(import.Type);
            }
            foreach(GlobalVariable variable in _module.EnumerateGlobalVariables()) {
                globals.Add(variable.Type);
            }
            _globalTypes = globals.ToArray();
        }

        private void InitializeLocalVariables(FunctionDefinition from)
        {
            _localTypes = from.Locals;
            return;
        }

        private void InitializeMemoryMap()
        {
            
        }

        internal void Reset(FunctionDefinition forFunction = null)
        {
            Errors.Clear();
            Labels.Clear();
            _stack.Clear();
            // Global variables are at module level. No reset required here.
            _currentFunction = forFunction;
            BuiltinLanguageType[] locals = _currentFunction.Locals;
            if (null != _currentFunction) {
                // TODO : Optimize this. This is way to much slow.
                List<BuiltinLanguageType> types = new List<BuiltinLanguageType>();
                foreach(BuiltinLanguageType type in _currentFunction.Signature.EnumerateParameters()) {
                    types.Add(type);
                }
                if (null != locals) { types.AddRange(locals); }
                _localTypes = types.ToArray();
            }
            else { _localTypes = _currentFunction.Locals ?? EmptyBuiltinLanguageType; }
        }

        internal BuiltinLanguageType StackPeek(int depth)
        {
            return GetStackItem(_stack.Count - depth - 1);
        }

        internal BuiltinLanguageType StackPop()
        {
            int stackIndex = _stack.Count - 1;
            try { return GetStackItem(stackIndex); }
            finally { _stack.RemoveAt(stackIndex); }
        }

        internal void StackPush(BuiltinLanguageType type)
        {
            _stack.Add(type);
        }

        internal bool ValidateElseOrEnd(OpCodes opcode)
        {
            if (0 == Labels.Count) {
                Errors.Add("End instruction encountered with no matching block, loop or if.");
                return false;
            }
            int labelTop = Labels.Count - 1;
            Tuple<BuiltinLanguageType, int, bool> label = Labels[labelTop];
            Labels.RemoveAt(labelTop);
            switch (opcode) {
                case OpCodes.Else:
                    if (label.Item3) { CreateLabel(label.Item1, false); }
                    else { Errors.Add("Else block encountered without maching if."); }
                    break;
                case OpCodes.End:
                    break;
                default:
                    throw new ApplicationException();
            }
            BuiltinLanguageType expectedType = label.Item1;
            int expectedBarrierDelta;
            if (BuiltinLanguageType.EmptyBlock != expectedType) {
                expectedBarrierDelta = 1;
                BuiltinLanguageType topOfStackType = StackPeek(0);
                if (expectedType != topOfStackType) {
                    Errors.Add(string.Format("Top of stack datatype {0} doesn't match End instruction expected type {1}.",
                        topOfStackType, expectedType));
                    return false;
                }
            }
            else { expectedBarrierDelta = 0; }
            int expectedStackSize = label.Item2 + expectedBarrierDelta;
            if (expectedStackSize != _stack.Count) {
                Errors.Add("Unbalanced stack encountered on End.");
                return false;
            }
            // Restore stack barrier to that of the previous block.
            _currentStackBarrier = (0 >= Labels.Count) ? 0 : Labels[Labels.Count - 1].Item2;
            return true;
        }

        internal bool ValidatePopableItemsCount(int count)
        {
            if (0 > count) { throw new ArgumentOutOfRangeException(); }
            int stackCount = _stack.Count;
            if (count > stackCount) {
                Errors.Add(string.Format("Attempt to pop on empty stack."));
                return false;
            }
            if ((stackCount - count) < _currentStackBarrier) {
                Errors.Add(string.Format("Attempt to pop a stack element beyond stack barrier."));
                return false;
            }
            return true;
        }

        private static readonly BuiltinLanguageType[] EmptyBuiltinLanguageType = new BuiltinLanguageType[0];
        private FunctionDefinition _currentFunction;
        /// <summary>This is used by blocks. Current specification states that each block must consider the
        /// stack to be empty on block entry. This member is the minimum index in the stack that can be
        /// accessed by the current block. This means that any attempt to access the stack must be performed
        /// while the Stack.Count property is greater or equal to this value.</summary>
        private int _currentStackBarrier = 0;
        private BuiltinLanguageType[] _globalTypes;
        private BuiltinLanguageType[] _localTypes;
        private WasmModule _module;
        private List<BuiltinLanguageType> _stack;
    }
}
