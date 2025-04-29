import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReservationService } from 'src/app/core/services/reservation.service';

@Component({
  selector: 'app-edit-reservation',
  templateUrl: './edit-reservation.component.html'
})
export class EditReservationComponent implements OnInit {
  reservationForm: FormGroup;
  id!: number;

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private reservationService: ReservationService,
    private router: Router
  ) {
    this.reservationForm = this.fb.group({
      id: [this.id],
      checkInDate: ['', Validators.required],
      checkOutDate: ['', Validators.required]
    }, { validators: this.checkDates });
  }

  async ngOnInit() {
    this.id = this.route.snapshot.params['id'];
    const reservation = await this.reservationService.getReservation(this.id).toPromise();
    this.reservationForm.patchValue(reservation);
  }

  async onSubmit() {
    if (this.reservationForm.valid) {
      try {
        const updatedReservation = this.reservationForm.value;
        await this.reservationService.updateReservation(updatedReservation);
        this.router.navigate(['/reservations']);
      } catch (error) {
        console.error('Error updating reservation:', error);
      }
    }
  }

  checkDates(group: FormGroup) {
    const checkIn = group.get('checkInDate')?.value;
    const checkOut = group.get('checkOutDate')?.value;
    return checkIn && checkOut && new Date(checkIn) >= new Date(checkOut) ? { datesInvalid: true } : null;
  }
  
}
