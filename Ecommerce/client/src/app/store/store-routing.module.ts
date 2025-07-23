import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import path from 'path';
import { StoreComponent } from './store.component';



const routes: Routes = [
  // Define your routes here
  {path:'store', loadChildren: () => import('./store.module').then(m => m.StoreModule)},
  {path: '**', redirectTo: '', pathMatch: 'full'},
];

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class StoreRoutingModule { }
