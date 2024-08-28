import { Component, inject, OnInit } from '@angular/core';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';


@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})

export class MemberEditComponent implements OnInit {
  member? : Member;
  
  user: User | null | undefined;
  private accountService = inject(AccountService);
  private memberService = inject(MembersService);
  
  constructor() {
    
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => this.user = user
    });
  }


  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    const user = this.user;
    if (!user) return;
    this.memberService.getMember(user.userName).subscribe({
      next: member => this.member = member
    })
  }

}