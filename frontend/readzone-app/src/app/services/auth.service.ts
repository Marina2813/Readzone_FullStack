import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';


interface User {
  username?: string,
  email: string;
  passwordHash: string;
}

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  private apiUrl = 'https://localhost:7216/api/Auth'; 

  constructor(private http: HttpClient) {}

   register(user: User): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, user);
  }

  login(user: User): Observable<{ token: string }> {
    return this.http.post<{ token: string }>(`${this.apiUrl}/login`, user);
  }

   getSecureData(): Observable<any> {
    const token = localStorage.getItem('token');
    const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : undefined;

    return this.http.get(this.apiUrl, { headers });
  }
  getUserId(): number | null {
  const token = localStorage.getItem('token');
  if (!token) return null;

  try {
    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload?.UserId ?? null;
  } catch (e) {
    console.error("Failed to decode token", e);
    return null;
  }
}

getUsernameById(userId: number): Observable<{ username: string }> {
  return this.http.get<{ username: string }>(`https://localhost:7216/api/Auth/user/${userId}`);
}


}
