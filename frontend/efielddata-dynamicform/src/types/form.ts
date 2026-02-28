export type FieldType = 'text' | 'number' | 'date' | 'dropdown' | 'multiselect' | 'checkbox' | 'radio' | 'file' | 'signature' | 'table';
export interface FormField { id: number; label: string; fieldName: string; fieldType: FieldType; isRequired: boolean; sortOrder: number; settingsJson: string; }
export interface FormSection { id: number; title: string; sortOrder: number; fields: FormField[]; }
export interface FormModel { id: number; name: string; isActive: boolean; sections: FormSection[]; }
