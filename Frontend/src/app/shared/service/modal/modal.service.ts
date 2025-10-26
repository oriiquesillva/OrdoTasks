import {
  ComponentRef,
  Injectable,
  Injector,
  Type,
  inject,
} from '@angular/core';
import { Overlay, OverlayRef } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';

export interface ModalRef<T> {
  overlayRef: OverlayRef;
  componentRef: ComponentRef<T>;
}

@Injectable({ providedIn: 'root' })
export class ModalService {
  private overlay = inject(Overlay);
  private injector = inject(Injector);

  openModal<T>(component: Type<T>): ModalRef<T> {
    const overlayRef = this.overlay.create({
      hasBackdrop: true,
      backdropClass: 'overlay-backdrop',
      panelClass: 'overlay-panel-center',
      positionStrategy: this.overlay
        .position()
        .global()
        .centerHorizontally()
        .centerVertically(),
    });

    const injector = Injector.create({
      providers: [{ provide: OverlayRef, useValue: overlayRef }],
      parent: this.injector,
    });

    const portal = new ComponentPortal(component, null, injector);
    const componentRef = overlayRef.attach(portal);

    overlayRef.backdropClick().subscribe(() => overlayRef.dispose());

    return { overlayRef, componentRef };
  }
}
