import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReservationService } from 'src/app/core/services/reservation.service';
import { Router } from '@angular/router';
import { Room, RoomService } from 'src/app/core/services/room.service';

@Component({
  selector: 'app-add-reservation',
  templateUrl: './add-reservation.component.html'
})
export class AddReservationComponent {
  reservationForm: FormGroup;
  rooms: Room[] = [];
  errorMessage: string = '';

  constructor(
    private fb: FormBuilder,
    private reservationService: ReservationService,
    private roomService: RoomService,
    private router: Router
  ) {
    this.reservationForm = this.fb.group({
      roomId: ['', Validators.required],
      checkInDate: ['', Validators.required],
      checkOutDate: ['', Validators.required]
    }, { validators: this.checkDates });
  }

  async ngOnInit() {
    this.rooms = await this.roomService.getRooms().toPromise();
  }

  
  onSubmit() {
    if (this.reservationForm.invalid) return;

    this.errorMessage = ''; // Clear old error
    this.reservationService.createReservation(this.reservationForm.value).subscribe({
      next: () => {
        this.router.navigate(['/reservations']);
      },
      error: (error) => {
          this.errorMessage = error.error
        
      }
    });
  }

  checkDates(group: FormGroup) {
    const checkIn = group.get('checkInDate')?.value;
    const checkOut = group.get('checkOutDate')?.value;
    return checkIn && checkOut && new Date(checkIn) >= new Date(checkOut) ? { datesInvalid: true } : null;
  }  
}
