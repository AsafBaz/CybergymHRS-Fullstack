import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from './base.service';
import { Observable } from 'rxjs';

export interface Reservation {
  id: number;
  userId: string;
  roomId: number;
  room?: {
    id: number;
    roomNumber: string;
    type: string;
    capacity: number;
    pricePerNight: number;
    isAvailable: boolean;
  };
  checkInDate: string;
  checkOutDate: string;
  status: string;
}


@Injectable({
  providedIn: 'root'
})
export class ReservationService extends BaseService {
  private reservationsUrl = `${this.baseApiUrl}/reservations`;

  constructor(private http: HttpClient) {
    super();
  }

  getReservations(): Observable<Reservation[]> {
    return this.http.get<Reservation[]>(this.reservationsUrl);
  }

  getReservation(id: number): Observable<Reservation> {
    return this.http.get<Reservation>(`${this.reservationsUrl}/${id}`);
  }

  createReservation(reservation: Reservation): Observable<Reservation> {
    console.log('here2')
    return this.http.post<Reservation>(this.reservationsUrl, reservation);
  }

  async updateReservation(reservation: Reservation): Promise<Reservation> {
    return this.http.put<Reservation>(`${this.reservationsUrl}/${reservation.id}`, reservation).toPromise();
  }

  deleteReservation(id: number): Observable<void> {
    return this.http.delete<void>(`${this.reservationsUrl}/${id}`);
  }
}
