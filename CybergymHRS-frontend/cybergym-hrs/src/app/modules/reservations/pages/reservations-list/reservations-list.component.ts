import { Component, OnInit } from '@angular/core';
import { ReservationService, Reservation } from 'src/app/core/services/reservation.service';

@Component({
  selector: 'app-reservations-list',
  templateUrl: './reservations-list.component.html',
  styleUrls: ['./reservations-list.component.scss']
})
export class ReservationsListComponent implements OnInit {
  reservations: Reservation[] = [];
  loading = false;
  displayedColumns: string[] = ['room', 'checkInDate', 'checkOutDate', 'status', 'actions'];

  constructor(private reservationService: ReservationService) {}

  ngOnInit(): void {
    this.loadReservations();
  }

  loadReservations() {
    this.loading = true;
    this.reservationService.getReservations().subscribe({
      next: (data) => {
        this.reservations = data;
      },
      error: (err) => {
        console.error(err);
      },
      complete: () => {
        this.loading = false;
      }
    });
  }

  deleteReservation(id: number) {
    if (confirm('Are you sure you want to delete this reservation?')) {
      this.reservationService.deleteReservation(id).subscribe(() => {
        this.loadReservations();
      });
    }
  }
}
