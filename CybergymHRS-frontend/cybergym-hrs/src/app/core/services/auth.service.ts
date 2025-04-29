import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from './base.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService extends BaseService {
  private authUrl = `${this.baseApiUrl}/account`;

  constructor(private http: HttpClient) {
    super();
  }

  isLoggedIn(): boolean {
    return localStorage.getItem('isLoggedIn') === 'true';
  }

  logout(): Promise<void> {
    return this.http.post(`${this.baseApiUrl}/account/logout`, {}, { withCredentials: true })
      .toPromise()
      .then(() => {
        localStorage.removeItem('isLoggedIn');
        localStorage.clear();
        sessionStorage.clear();
      })
      .catch(error => {
        console.error('Logout error:', error);
        localStorage.removeItem('isLoggedIn');
        localStorage.clear();
        sessionStorage.clear();
      });
  }
  
  
  login(email: string, password: string): Promise<any> {
    const response = this.http.post(`${this.authUrl}/login`, { email, password }, { withCredentials: true }).toPromise();
    localStorage.setItem('isLoggedIn', 'true');
    return response;
  }

  register(fullName: string, email: string, password: string): Promise<any> {
    return this.http.post(`${this.authUrl}/register`, { fullName, email, password }, { withCredentials: true }).toPromise();
  }
}
