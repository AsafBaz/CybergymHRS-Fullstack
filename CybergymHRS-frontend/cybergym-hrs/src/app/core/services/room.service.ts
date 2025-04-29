import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from './base.service';
import { Observable } from 'rxjs';

export interface Room {
  id: number;
  roomNumber: string;
  type: string;
  capacity: number;
  pricePerNight: number;
  isAvailable: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class RoomService extends BaseService {
  private roomsUrl = `${this.baseApiUrl}/rooms`;

  constructor(private http: HttpClient) {
    super();
  }

  getRooms(): Observable<Room[]> {
    return this.http.get<Room[]>(this.roomsUrl);
  }

  getRoom(id: number): Observable<Room> {
    return this.http.get<Room>(`${this.roomsUrl}/${id}`);
  }

  createRoom(room: Room): Observable<Room> {
    return this.http.post<Room>(this.roomsUrl, room);
  }

  updateRoom(room: Room): Observable<Room> {
    return this.http.put<Room>(`${this.roomsUrl}/${room.id}`, room);
  }

  deleteRoom(id: number): Observable<void> {
    return this.http.delete<void>(`${this.roomsUrl}/${id}`);
  }
}
