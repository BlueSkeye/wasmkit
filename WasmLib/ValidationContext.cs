﻿using System;
using System.Collections.Generic;

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
            InitializeFunctions();
            InitializeMemoryMap();
            InitializeTables();
        }

        internal List<string> Errors { get; private set; }

        internal BuiltinLanguageType FunctionReturnType
        {
            get { return _currentFunction.Signature.ReturnType; }
        }

        internal int LabelsCount
        {
            get { return Labels.Count; }
        }

        private List<Tuple<BuiltinLanguageType, int, bool>> Labels { get; set; }

        internal bool LastInstructionWasReturn { get; set; }

        internal int StackSize
        {
            get { return _stack.Count; }
        }

        internal void AddError(string message)
        {
            Errors.Add(message);
            return;
        }

        internal void CreateLabel(BuiltinLanguageType type, bool allowReuseForElse)
        {
            Labels.Add(new Tuple<BuiltinLanguageType, int, bool>(type, (_currentStackBarrier = _stack.Count), allowReuseForElse));
        }

        internal FunctionSignature GetFunctionSignature(uint index)
        {
            if (index >= _functions.Length) {
                this.AddError(string.Format("Attempt to retrieve function #{0} while there is only {1} functions.",
                    index, _functions.Length));
                return null;
            }
            return _functions[(int)index];
        }

        internal BuiltinLanguageType GetGlobalVariableType(uint index)
        {
            if (_globalTypes.Length <= index) {
                Errors.Add(string.Format("{0} is an invalid global variable index.", index));
                return 0;
            }
            return _globalTypes[index];
        }

        internal FunctionSignature GetIndirectFunctionSignature(uint index)
        {
            if (0 > _tables.Length) {
                Errors.Add("Indirect function signature can't be resolved because no table is defined.");
                return null;
            }
            if (BuiltinLanguageType.AnyFunc != _tables[0].ElementType) {
                Errors.Add("Indirect function signature can't be resolved because type of table[0] is not AnyFunc.");
                return null;
            }
            FunctionSignature result = _module.GetFunctionSignature(index);
            if (null == result) {
                Errors.Add(string.Format("Indirect function index {0} is greater than maximum allowed index.",
                    index));
                return null;
            }
            return result;
        }

        /// <summary></summary>
        /// <param name="relativeLabelIndex">The index is relative to the end. That is, 1 means
        /// the most recently added label.</param>
        /// <param name="label"></param>
        /// <returns></returns>
        internal bool GetLabel(uint relativeLabelIndex, out Tuple<BuiltinLanguageType, int, bool> label)
        {
            if (0 > relativeLabelIndex) { throw new ArgumentOutOfRangeException(); }
            int labelIndex = (int)(Labels.Count - relativeLabelIndex - 1);
            if (0 > labelIndex) {
                label = default(Tuple<BuiltinLanguageType, int, bool>);
                return false;
            }
            label = Labels[labelIndex];
            return true;
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
            if (0 > stackIndex) {
                Errors.Add(string.Format("Attempt to peek a stack element out of stack range."));
                return 0;
            }
            if (stackIndex < _currentStackBarrier) {
                Errors.Add(string.Format("Attempt to pop a stack element beyond stack barrier."));
                return 0;
            }
            return _stack[stackIndex];
        }

        private void InitializeFunctions()
        {
            List<FunctionSignature> functions = new List<FunctionSignature>();

            foreach(ImportedFunctionDefinition function in _module.EnumerateFunctionImports()) {
                functions.Add(_module.GetFunctionSignature(function.FunctionSignatureIndex));
            }
            foreach(FunctionDefinition function in _module.EnumerateFunctions()) {
                functions.Add(function.Signature);
            }
            _functions = functions.ToArray();
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

        private void InitializeTables()
        {
            List<ImportedTableDefinition> tables = new List<ImportedTableDefinition>();

            foreach(ImportedTableDefinition table in _module.EnumerateGlobalTables()) {
                tables.Add(table);
            }
            _tables = tables.ToArray();
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

        internal string StackLabelBarrier()
        {
            return string.Format("S={0},L={1},B={2}",
                _stack.Count, Labels.Count, _currentStackBarrier);
        }

        internal BuiltinLanguageType StackPeek(int depth)
        {
            return GetStackItem(_stack.Count - depth - 1);
        }

        internal BuiltinLanguageType StackPop()
        {
            int stackIndex = _stack.Count - 1;
            if (0 == _stack.Count) {
                return 0;
            }
            BuiltinLanguageType result = GetStackItem(stackIndex);
            if (0 != result) { _stack.RemoveAt(stackIndex); }
            return result;
        }

        internal void StackPush(BuiltinLanguageType type)
        {
            _stack.Add(type);
        }

        internal bool ValidateElseOrEnd(OpCodes opcode)
        {
            if (0 == Labels.Count) {
                Errors.Add("End or Else instruction encountered with no matching block, loop or if.");
                return false;
            }
            int labelTop = Labels.Count - 1;
            Tuple<BuiltinLanguageType, int, bool> label = Labels[labelTop];
            Labels.RemoveAt(labelTop);
            int previousStackBarrier = (0 >= Labels.Count) ? 0 : Labels[Labels.Count - 1].Item2;
            BuiltinLanguageType expectedType = label.Item1;
            if (!LastInstructionWasReturn) {
                if (BuiltinLanguageType.EmptyBlock != expectedType) {
                    // Remove the block result from the stack for validation purpose, otherwise
                    // this would let us with an  unbalanced stack after Else completion later on.
                    BuiltinLanguageType poppedType = StackPop();
                    if (0 == poppedType) { return false; }
                    if (expectedType != poppedType) {
                        Errors.Add(string.Format("Top of stack datatype {0} doesn't match End or Else instruction expected type {1}.",
                            poppedType, expectedType));
                        return false;
                    }
                }
                if (label.Item2 != _stack.Count) {
                    Errors.Add("Unbalanced stack encountered on Else or End.");
                    return false;
                }
            }
            switch (opcode) {
                case OpCodes.Else:
                    if (label.Item3) { CreateLabel(label.Item1, false); }
                    else { Errors.Add("Else block encountered without maching If."); }
                    break;
                case OpCodes.End:
                    // Restore stack barrier to that of the previous block.
                    _currentStackBarrier = previousStackBarrier;
                    // Push back block result if any.
                    if (BuiltinLanguageType.EmptyBlock != expectedType) {
                        StackPush(expectedType);
                    }
                    break;
                default:
                    throw new ApplicationException();
            }
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
        private FunctionSignature[] _functions;
        private BuiltinLanguageType[] _globalTypes;
        private BuiltinLanguageType[] _localTypes;
        private WasmModule _module;
        private List<BuiltinLanguageType> _stack;
        private ImportedTableDefinition[] _tables;
    }
}
