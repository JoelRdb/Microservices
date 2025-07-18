import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { StoreComponent } from './store/store.component';
import { ProductDetailsComponent } from './store/product-details/product-details.component';

export const routes: Routes = [
    {path: '', component:HomeComponent},
    {path: 'store', component:StoreComponent},
    {path: 'store/:id', component:ProductDetailsComponent},
    {path: '**', redirectTo: '', pathMatch: 'full'},

];
