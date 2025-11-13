import { Injectable, NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { MyorderComponent } from "./myorder.component";



const routes : Routes = [
    { 
        path: '', 
        component: MyorderComponent
    }
]

@NgModule({
    declarations: [],
    imports: [
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})
export class MyOrderRoutingModule{

}