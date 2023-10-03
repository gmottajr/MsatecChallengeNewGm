import {Telefone} from './telefone';

export interface ClienteForLists {
    id: string;
    nome: string;
    email: string;
    telefonePrincipal: Telefone;
}
