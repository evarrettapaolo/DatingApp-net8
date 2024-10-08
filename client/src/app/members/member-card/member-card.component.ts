import { Component, input, ViewEncapsulation } from '@angular/core';
import { Member } from '../../_models/member';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-member-card',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './member-card.component.html',
  styles: [
    `
      .card:hover img {
        transform: scale(1.2, 1.2);
        transition-duration: 500ms;
        transition-timing-function: ease-out;
      }

      .card img {
        transform: scale(1, 1);
        transition-duration: 500ms;
        transition-timing-function: ease-out;
      }

      .card-img-wrapper {
        overflow: hidden;
        position: relative;
      }

      .member-icons {
        position: absolute;
        bottom: -30%;
        left: 0;
        right: 0;
        margin-right: auto;
        margin-left: auto;
        opacity: 0;
      }

      .card-img-wrapper:hover .member-icons {
        bottom: 0;
        opacity: 1;
      }

      .animate {
        transition: all 0.3s ease-in-out;
      }
    `,
  ],
})
export class MemberCardComponent {
  member = input.required<Member>();
}
