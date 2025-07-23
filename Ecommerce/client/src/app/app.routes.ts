import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { StoreComponent } from './store/store.component';
import { ProductDetailsComponent } from './store/product-details/product-details.component';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { UnAuthenticatedComponent } from './core/un-authenticated/un-authenticated.component';
import { NotFoundComponent } from './core/not-found/not-found.component';


export const routes: Routes = [
    {path: '', component:HomeComponent},
    {path: 'un-authenticated', component: UnAuthenticatedComponent},
    {path: 'not-found', component: NotFoundComponent},
    {path: 'server-error', component: ServerErrorComponent},
    {path: 'store', component:StoreComponent},
    {path: 'store/:id', component:ProductDetailsComponent},
    {path: '**', redirectTo: '', pathMatch: 'full'}

];
