import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CadastroClienteComponent } from './cadastro-cliente/cadastro-cliente.component';
import { HomeComponent } from './home/home.component';

const routes: Routes = [ 
  {path: 'home', component: HomeComponent},
  {path: '', component: HomeComponent},
  {path: 'cadastro-cliente', component: CadastroClienteComponent} ,
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
