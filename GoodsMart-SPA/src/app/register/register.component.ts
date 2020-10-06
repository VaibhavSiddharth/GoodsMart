import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  @Output() eventEmitter = new EventEmitter();
  model: any = {};
  constructor(private authService: AuthService) {}

  ngOnInit(): void {}

  register() {
    this.authService.register(this.model).subscribe(
      () => {
        console.log('Registration successful');
      },
      (error) => {
        console.log(error);
      }
    );
  }

  cancel() {
    this.eventEmitter.emit(false);
    console.log('Cancelled');
  }
}
