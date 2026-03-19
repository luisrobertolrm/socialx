import api from './api';
import { EntidadeTesteDto } from '../types/EntidadeTesteDto';

export const entidadeTesteService = {
  obterPorId: async (id: number): Promise<EntidadeTesteDto> => {
    const response = await api.get<EntidadeTesteDto>(`/EntidadeTeste/${id}`);
    return response.data;
  },
};
