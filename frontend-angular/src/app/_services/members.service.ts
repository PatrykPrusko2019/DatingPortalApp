import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members = signal<Member[]>([]);

  constructor(private http: HttpClient) { }

  getMembers() {
    return this.http.get<Member[]>(this.baseUrl + 'user').subscribe({
      next: members => this.members.set(members)
    })
  }

  getMember(userName: string) {
    const member = this.members().find(x => x.userName == userName);
    if (member !== undefined) return of(member);

    return this.http.get<Member>(this.baseUrl + 'user/' + userName);
  }

  updateMember(member: Member) {
    return this.http.put(this.baseUrl + 'user', member).pipe(
      tap(() => {
        this.members.update(members => members.map(m => m.userName === member.userName ? member : m))
      } 
      )
    )
  }


}
