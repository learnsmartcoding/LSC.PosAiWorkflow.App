import { DatePipe } from '@angular/common';
import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { WorkflowUiStore } from '../../../core/services/workflow-ui.store';


@Component({
  selector: 'app-trace-view',
  standalone: true,
  imports: [RouterLink, DatePipe],
  templateUrl: './trace-view.component.html'
})
export class TraceViewComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  readonly store = inject(WorkflowUiStore);

  readonly correlationId = signal('');
  readonly events = computed(() => this.store.traceEvents());
  readonly isLoading = computed(() => this.store.traceLoading());
  readonly errorMessage = computed(() => this.store.traceError());

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('correlationId');

      if (!id) {
        this.correlationId.set('');
        return;
      }

      this.correlationId.set(id);
      void this.store.loadTrace(id);
    });
  }

  refresh(): void {
    const id = this.correlationId();
    if (id) {
      void this.store.loadTrace(id);
    }
  }
}