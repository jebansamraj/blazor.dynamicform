import { configureStore } from '@reduxjs/toolkit';
import authReducer from '../slices/authSlice';
import formBuilderReducer from '../slices/formBuilderSlice';
import formReducer from '../slices/formSlice';
import submissionReducer from '../slices/submissionSlice';

export const store = configureStore({ reducer: { auth: authReducer, formBuilder: formBuilderReducer, forms: formReducer, submissions: submissionReducer } });
export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
