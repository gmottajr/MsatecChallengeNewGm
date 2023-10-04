import {Telefone} from './telefone';

export interface ClienteForLists {
    id: string;
    nome: string;
    email: string;
    telefonePrincipal: Telefone;
}

export interface Cliente {
    id: string;
    nome: string;
    email: string;
    dataNascimento?: Date | null;
    telefones: Telefone[];
    isInserting: boolean;
  }