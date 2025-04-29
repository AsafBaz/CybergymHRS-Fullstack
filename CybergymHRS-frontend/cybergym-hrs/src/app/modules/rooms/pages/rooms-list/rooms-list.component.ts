import { Component, OnInit } from '@angular/core';
import { RoomService, Room } from 'src/app/core/services/room.service';

@Component({
  selector: 'app-rooms-list',
  templateUrl: './rooms-list.component.html',
  styleUrls: ['./rooms-list.component.scss']
})
export class RoomsListComponent implements OnInit {
  rooms: Room[] = [];
  loading = false;
  displayedColumns: string[] = ['roomNumber', 'type', 'capacity', 'pricePerNight', 'actions'];

  constructor(private roomService: RoomService) {}

  ngOnInit(): void {
    this.loadRooms();
  }

  loadRooms() {
    this.loading = true;
    this.roomService.getRooms().subscribe({
      next: (data) => {
        this.rooms = data;
      },
      error: (err) => {
        console.error(err);
      },
      complete: () => {
        this.loading = false;
      }
    });
  }

  deleteRoom(id: number) {
    if (confirm('Are you sure you want to delete this room?')) {
      this.roomService.deleteRoom(id).subscribe(() => {
        this.loadRooms(); // reload after delete
      });
    }
  }
}
