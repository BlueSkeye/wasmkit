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
            Labels = new Stack<Tuple<sbyte, int>>();
            _stack = new Stack<BuiltinLanguageType>();
            InitializeGlobalVariables();
        }

        internal List<string> Errors { get; private set; }

        private Stack<Tuple<sbyte, int>> Labels { get; set; }

        internal void AddError(string message)
        {
            Errors.Add(message);
            return;
        }

        internal void CreateLabel(sbyte type)
        {
            Labels.Push(new Tuple<sbyte, int>(type, (_currentStackBarrier = _stack.Count)));
        }

        internal BuiltinLanguageType GetGlobalVariableType(uint index)
        {
            if (_globalTypes.Length <= index) {
                Errors.Add(string.Format("{0} is an invalid global variable index."));
                return 0;
            }
            return _globalTypes[index];
        }

        internal BuiltinLanguageType GetLocalVariableType(uint index)
        {
            if (_localTypes.Length <= index) {
                Errors.Add(string.Format("{0} is an invalid local variable index."));
                return 0;
            }
            return _localTypes[index];
        }

        private void InitializeGlobalVariables()
        {
            int globalVariableCount = _module.GlobalsCount;
            _globalTypes = new BuiltinLanguageType[globalVariableCount];
            int globalVariableIndex = 0;
            foreach(GlobalVariable variable in _module.EnumerateGlobalVariables()) {
                _globalTypes[globalVariableIndex++] = variable.Type;
            }
        }

        private void InitializeLocalVariables(FunctionDefinition from)
        {
            int globalVariableCount = _module.GlobalsCount;
            _globalTypes = new BuiltinLanguageType[globalVariableCount];
            int globalVariableIndex = 0;
            foreach(GlobalVariable variable in _module.EnumerateGlobalVariables()) {
                _globalTypes[globalVariableIndex++] = variable.Type;
            }
        }

        internal void Reset(FunctionDefinition forFunction = null)
        {
            Errors.Clear();
            Labels.Clear();
            _stack.Clear();
            // Global variables are at module level. No reset required here.
            _currentFunction = forFunction;
            if (null != _currentFunction) { _localTypes = _currentFunction.Locals; }
        }

        internal BuiltinLanguageType StackPop()
        {
            if (_stack.Count <= _currentStackBarrier) {
                Errors.Add(string.Format("Attempt to pop a stack element beyond stack barrier."));
                return 0;
            }
            return _stack.Pop();
        }

        internal void StackPush(BuiltinLanguageType type)
        {
            _stack.Push(type);
        }

        private FunctionDefinition _currentFunction;
        /// <summary>This is used by blocks. Current specification states that each blockmust consider the
        /// stack to be empty on block entry. This member is the minimum index in the stack that can be
        /// accessed by the current block. This means that any attempt to access the stack must be performed
        /// while the Stack.Count property is greater than this value.</summary>
        private int _currentStackBarrier = 0;
        private BuiltinLanguageType[] _globalTypes;
        private BuiltinLanguageType[] _localTypes;
        private WasmModule _module;
        private Stack<BuiltinLanguageType> _stack;
    }
}
