import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RoomService } from 'src/app/core/services/room.service';

@Component({
  selector: 'app-add-room',
  templateUrl: './add-room.component.html',
  styleUrls: ['./add-room.component.scss']
})
export class AddRoomComponent {
  roomForm: FormGroup;
  roomTypes: string[] = ['Single', 'Double', 'Twin', 'Suite', 'Deluxe', 'Family'];
  errorMessage: string = '';

  constructor(
    private fb: FormBuilder,
    private roomService: RoomService,
    private router: Router
  ) {
    this.roomForm = this.fb.group({
      roomNumber: ['', Validators.required],
      type: ['', Validators.required],
      capacity: [0, Validators.required],
      pricePerNight: [0, Validators.required],
      isAvailable: [true]
    });
  }

  onSubmit() {
    if (this.roomForm.invalid) return;

    this.roomService.createRoom(this.roomForm.value).subscribe({
      next: () => {
        this.router.navigate(['/rooms']);
      },
      error: (error) => {
        console.log(error)
          this.errorMessage = error.error.error
        
      }
    });
  }
}
