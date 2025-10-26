import { Injectable } from '@angular/core';
import Swal, { SweetAlertIcon } from 'sweetalert2';

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  show(
    icon: SweetAlertIcon,
    message: string,
    position: 'top-end' | 'bottom-end' | 'top' | 'bottom' = 'top-end'
  ) {
    Swal.fire({
      toast: true,
      position,
      icon,
      title: message,
      showConfirmButton: false,
      timer: 3000,
      timerProgressBar: true,
      background: '#ffffff',
      color: '#333333',
      customClass: {
        popup: 'swal-toast',
      },
    });
  }

  success(message: string) {
    this.show('success', message);
  }

  warning(message: string) {
    this.show('warning', message);
  }

  error(message: string) {
    this.show('error', message);
  }

  info(message: string) {
    this.show('info', message);
  }
}
