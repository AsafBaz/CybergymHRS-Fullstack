import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReservationsListComponent } from './pages/reservations-list/reservations-list.component';
import { AddReservationComponent } from './pages/add-reservation/add-reservation.component';
import { EditReservationComponent } from './pages/edit-reservation/edit-reservation.component';

const routes: Routes = [
  { path: '', component: ReservationsListComponent },
  { path: 'add', component: AddReservationComponent },
  { path: 'edit/:id', component: EditReservationComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReservationsRoutingModule { }
