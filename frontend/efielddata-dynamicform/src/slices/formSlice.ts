import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { FormModel } from '../types/form';
const slice = createSlice({ name: 'forms', initialState: { items: [] as FormModel[] }, reducers: { setForms: (s, a: PayloadAction<FormModel[]>) => { s.items = a.payload; } } });
export const { setForms } = slice.actions;
export default slice.reducer;
