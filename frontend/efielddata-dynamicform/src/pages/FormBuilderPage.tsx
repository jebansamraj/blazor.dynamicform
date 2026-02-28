import { Box, Button, Container, Grid2, List, ListItemButton, MenuItem, Paper, Stack, Switch, TextField, Typography } from '@mui/material';
import { DndProvider, useDrag, useDrop } from 'react-dnd';
import { HTML5Backend } from 'react-dnd-html5-backend';
import { useMemo } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate, useParams } from 'react-router-dom';
import { api } from '../api/client';
import { RootState } from '../app/store';
import { addField, deleteField, reorderField, selectField, updateField } from '../slices/formBuilderSlice';
import { FormField } from '../types/form';

const fieldTypes = ['text','number','date','dropdown','multiselect','checkbox','radio','file','signature','table'] as const;
function DraggableField({ field, index }: { field: FormField; index: number }) {
  const dispatch = useDispatch();
  const [, drag] = useDrag(() => ({ type: 'FIELD', item: { index } }), [index]);
  const [, drop] = useDrop(() => ({ accept: 'FIELD', hover: (item: { index: number }) => { if (item.index !== index) { dispatch(reorderField({ from: item.index, to: index })); item.index = index; } } }), [index]);
  return <ListItemButton ref={(node) => drag(drop(node))} onClick={() => dispatch(selectField(field.id))}>{field.label}</ListItemButton>;
}

export default function FormBuilderPage() {
  const { id } = useParams(); const dispatch = useDispatch(); const navigate = useNavigate();
  const { fields, selectedFieldId } = useSelector((s: RootState) => s.formBuilder);
  const selected = useMemo(() => fields.find(f => f.id === selectedFieldId) ?? null, [fields, selectedFieldId]);
  const add = (fieldType: FormField['fieldType']) => dispatch(addField({ id: Date.now(), label: `${fieldType} field`, fieldName: `${fieldType}_${Date.now()}`, fieldType, isRequired: false, sortOrder: fields.length + 1, settingsJson: '{}' }));
  const save = async () => {
    const payload = { name: `Form ${id ?? ''}`.trim(), isActive: true, sections: [{ id: 0, title: 'Section 1', sortOrder: 1, fields }] };
    if (id) await api.put(`/forms/${id}`, payload); else await api.post('/forms', payload);
    navigate('/');
  };
  return <DndProvider backend={HTML5Backend}><Container sx={{ py: 2 }}><Typography variant='h4' mb={2}>Form Builder</Typography><Grid2 container spacing={2}><Grid2 size={{ xs:12, md:3 }}><Paper sx={{ p:2 }}><Stack spacing={1}>{fieldTypes.map(t => <Button key={t} variant='outlined' onClick={() => add(t)}>{t}</Button>)}</Stack></Paper></Grid2><Grid2 size={{ xs:12, md:5 }}><Paper sx={{ p:2 }}><List>{fields.map((f, i) => <DraggableField key={f.id} field={f} index={i} />)}</List></Paper></Grid2><Grid2 size={{ xs:12, md:4 }}><Paper sx={{ p:2 }}>{selected ? <Stack spacing={1}><Typography>Field Settings</Typography><TextField label='Label' value={selected.label} onChange={e => dispatch(updateField({ ...selected, label: e.target.value }))} /><TextField label='Field Name' value={selected.fieldName} onChange={e => dispatch(updateField({ ...selected, fieldName: e.target.value }))} /><TextField select label='Type' value={selected.fieldType} onChange={e => dispatch(updateField({ ...selected, fieldType: e.target.value as FormField['fieldType'] }))}>{fieldTypes.map(t => <MenuItem key={t} value={t}>{t}</MenuItem>)}</TextField><Box><Typography>Required</Typography><Switch checked={selected.isRequired} onChange={e => dispatch(updateField({ ...selected, isRequired: e.target.checked }))} /></Box><TextField label='JSON settings' multiline minRows={4} value={selected.settingsJson} onChange={e => dispatch(updateField({ ...selected, settingsJson: e.target.value }))} /><Button color='error' onClick={() => dispatch(deleteField(selected.id))}>Delete</Button></Stack> : <Typography>Select a field</Typography>}</Paper></Grid2></Grid2><Button sx={{ mt: 2 }} variant='contained' onClick={save}>Save</Button></Container></DndProvider>;
}
