import { CommonModule } from '@angular/common';
import { Component, inject, NgModule, OnInit, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})

export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm?: NgForm;
  member? : Member;
  user: User | null | undefined;
  private accountService = inject(AccountService);
  private memberService = inject(MembersService);
  private toastr = inject(ToastrService)
  
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

  updateMember() {
    console.log(this.member);
    this.toastr.success('Profile updated succesfully');
    this.editForm?.reset(this.member);
  }

}