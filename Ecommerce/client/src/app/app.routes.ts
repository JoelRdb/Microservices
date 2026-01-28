import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { StoreComponent } from './store/store.component';
import { ProductDetailsComponent } from './store/product-details/product-details.component';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { UnAuthenticatedComponent } from './core/un-authenticated/un-authenticated.component';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { SigninRedirectCallbackComponent } from './account/signin-redirect-callback/signin-redirect-callback.component';
import { SignoutRedirectCallbackComponent } from './account/signout-redirect-callback/signout-redirect-callback.component';
import { AuthGuard } from './core/guards/auth.guard';



export const routes: Routes = [
    {path: '', component:HomeComponent, data : {breadcrumb: 'Home'} },
    {path: 'un-authenticated', component: UnAuthenticatedComponent},
    {path: 'not-found', component: NotFoundComponent},
    {path: 'server-error', component: ServerErrorComponent},
    {path: 'signin-callback', component: SigninRedirectCallbackComponent},
    {path: 'signout-callback', component: SignoutRedirectCallbackComponent},
    {path: 'store', loadChildren: () => import('./store/store.module').then(mod=>mod.StoreModule), data:{breadcrumb:'Store'}},
    {path: 'basket', loadChildren: () => import('./basket/basket.module').then(mod=>mod.BasketModule), data:{breadcrumb:'Basket'}},
    {path: 'account', loadChildren: () => import('./account/account.module').then(mod=> mod.AccountModule), data:{breadcrumb: {skip:true}}},
    {path: 'payment-information', loadChildren: () => import('./payment-information/payment-information.module').then(mod => mod.PaymentInformationModule)},
    {path: 'myorder', loadChildren: () => import('./myorder/myorder.module').then(mod => mod.MyOrderModule)},
    {path: 'checkout', canActivate:[AuthGuard], loadComponent:()=>import('./checkout/checkout.component').then(mod => mod.CheckoutComponent), data:{breadcrumb:'Checkout'}},
    {path: '**', redirectTo: '', pathMatch: 'full'}
];
