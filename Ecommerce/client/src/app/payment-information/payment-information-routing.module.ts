import { RouterModule, Routes } from "@angular/router";
import { PaymentInformationComponent } from "./payment-information";
import { NgModule } from "@angular/core";

const routes : Routes = [
    {
        path: '',
        component: PaymentInformationComponent
    }
]

@NgModule({
    declarations: [],
    imports: [
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})
export class PaymentInformationRoutingModule{
    
}