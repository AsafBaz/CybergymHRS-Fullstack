import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoomsListComponent } from './pages/rooms-list/rooms-list.component';
import { AddRoomComponent } from './pages/add-room/add-room.component';
import { EditRoomComponent } from './pages/edit-room/edit-room.component';

const routes: Routes = [
  { path: '', component: RoomsListComponent },
  { path: 'add', component: AddRoomComponent },
  { path: 'edit/:id', component: EditRoomComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RoomsRoutingModule { }
