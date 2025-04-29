import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RoomService } from 'src/app/core/services/room.service';

@Component({
  selector: 'app-edit-room',
  templateUrl: './edit-room.component.html',
  styleUrls: ['./edit-room.component.scss']
})
export class EditRoomComponent implements OnInit {
  roomForm: FormGroup;
  id: number;
  errorMessage: string = '';

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private roomService: RoomService,
    private router: Router
  ) {
    this.id = Number(this.route.snapshot.paramMap.get('id'));
    this.roomForm = this.fb.group({
      id: [this.id],
      roomNumber: ['', Validators.required],
      type: ['', Validators.required],
      capacity: [0, Validators.required],
      pricePerNight: [0, Validators.required],
      isAvailable: [true]
    });
  }

  ngOnInit(): void {
    this.roomService.getRoom(this.id).subscribe(room => {
      this.roomForm.patchValue(room);
    });
  }

  onSubmit() {
    if (this.roomForm.invalid) return;

    this.roomService.updateRoom(this.roomForm.value).subscribe({
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
