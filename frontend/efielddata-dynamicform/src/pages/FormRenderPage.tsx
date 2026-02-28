import { Button, Container, FormControlLabel, MenuItem, Stack, Switch, TextField, Typography } from '@mui/material';
import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { api } from '../api/client';
import { FormModel } from '../types/form';

export default function FormRenderPage() {
  const { id } = useParams(); const navigate = useNavigate();
  const [form, setForm] = useState<FormModel>();
  const [values, setValues] = useState<Record<number, any>>({});
  useEffect(() => { api.get(`/forms/${id}`).then(r => setForm(r.data)); }, [id]);
  const submit = async () => { await api.post(`/forms/${id}/submit`, { values: Object.entries(values).map(([formFieldId, value]) => ({ formFieldId: Number(formFieldId), value: String(value) })) }); navigate(`/forms/${id}/submissions`); };
  return <Container sx={{ py: 3 }}><Stack spacing={2}><Typography variant='h4'>{form?.name}</Typography><Button onClick={() => navigate(`/builder/${id}`)}>Edit Mode</Button>{form?.sections.flatMap(s => s.fields).map(f => {
    const val = values[f.id] ?? '';
    if (f.fieldType === 'checkbox') return <FormControlLabel key={f.id} control={<Switch checked={!!val} onChange={e => setValues(v => ({ ...v, [f.id]: e.target.checked }))} />} label={f.label} />;
    if (f.fieldType === 'dropdown' || f.fieldType === 'radio') {
      const options = JSON.parse(f.settingsJson || '{}').options ?? [];
      return <TextField key={f.id} select label={f.label} value={val} onChange={e => setValues(v => ({ ...v, [f.id]: e.target.value }))}>{options.map((o:string)=><MenuItem key={o} value={o}>{o}</MenuItem>)}</TextField>;
    }
    return <TextField key={f.id} type={f.fieldType === 'number' ? 'number' : 'text'} label={f.label} value={val} onChange={e => setValues(v => ({ ...v, [f.id]: e.target.value }))} required={f.isRequired} />;
  })}<Button variant='contained' onClick={submit}>Submit</Button></Stack></Container>;
}
