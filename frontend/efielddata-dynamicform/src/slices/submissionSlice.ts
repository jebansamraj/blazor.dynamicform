import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface SubmissionSummary { id:number; submittedAt:string; status:string; }
const slice = createSlice({ name:'submissions', initialState:{ items: [] as SubmissionSummary[] }, reducers:{ setSubmissions:(s,a:PayloadAction<SubmissionSummary[]>)=>{s.items=a.payload;} } });
export const { setSubmissions } = slice.actions;
export default slice.reducer;
