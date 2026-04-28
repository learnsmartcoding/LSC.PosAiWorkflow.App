import { DatePipe } from '@angular/common';
import { Component, OnInit, computed, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { WorkflowUiStore } from '../../core/services/workflow-ui.store';

@Component({
  selector: 'app-decisions-list',
  standalone: true,
  imports: [RouterLink, DatePipe],
  templateUrl: './decisions-list.component.html'
})
export class DecisionsListComponent implements OnInit {
  readonly store = inject(WorkflowUiStore);

  readonly decisions = computed(() => this.store.decisions());
  readonly isLoading = computed(() => this.store.decisionsLoading());
  readonly errorMessage = computed(() => this.store.decisionsError());

  ngOnInit(): void {
    void this.store.loadDecisions();
  }

  refresh(): void {
    void this.store.loadDecisions();
  }
}