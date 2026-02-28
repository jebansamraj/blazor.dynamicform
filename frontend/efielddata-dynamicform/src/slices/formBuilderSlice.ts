import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { FormField } from '../types/form';

interface BuilderState { fields: FormField[]; selectedFieldId: number | null; }
const initialState: BuilderState = { fields: [], selectedFieldId: null };
const slice = createSlice({
  name: 'formBuilder', initialState,
  reducers: {
    setFields: (s, a: PayloadAction<FormField[]>) => { s.fields = a.payload; },
    addField: (s, a: PayloadAction<FormField>) => { s.fields.push(a.payload); s.selectedFieldId = a.payload.id; },
    updateField: (s, a: PayloadAction<FormField>) => { const i = s.fields.findIndex(f => f.id === a.payload.id); if (i >= 0) s.fields[i] = a.payload; },
    deleteField: (s, a: PayloadAction<number>) => { s.fields = s.fields.filter(f => f.id !== a.payload); if (s.selectedFieldId === a.payload) s.selectedFieldId = null; },
    reorderField: (s, a: PayloadAction<{from:number;to:number}>) => { const [m] = s.fields.splice(a.payload.from, 1); s.fields.splice(a.payload.to, 0, m); s.fields = s.fields.map((f, i)=>({...f, sortOrder:i+1})); },
    selectField: (s,a:PayloadAction<number|null>) => { s.selectedFieldId = a.payload; }
  }
});
export const { setFields, addField, updateField, deleteField, reorderField, selectField } = slice.actions;
export default slice.reducer;
