import { Routes } from '@angular/router';
import { HomeComponent } from '../app/home/home.component';
import { GoodsListComponent } from '../app/goods-list/goods-list.component';
import { OrderListComponent } from '../app/order-list/order-list.component';
import { AuthGuard } from './_guards/auth.guard';

export const appRoutes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      { path: 'goods', component: GoodsListComponent },
      { path: 'orders', component: OrderListComponent },
    ],
  },

  { path: '**', redirectTo: '', pathMatch: 'full' },
];
