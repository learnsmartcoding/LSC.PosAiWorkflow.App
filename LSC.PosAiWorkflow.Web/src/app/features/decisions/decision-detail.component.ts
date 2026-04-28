import { DatePipe } from '@angular/common';
import { Component, OnDestroy, OnInit, computed, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { WorkflowUiStore } from '../../core/services/workflow-ui.store';

@Component({
  selector: 'app-decision-detail',
  standalone: true,
  imports: [RouterLink, DatePipe],
  templateUrl: './decision-detail.component.html'
})
export class DecisionDetailComponent implements OnInit, OnDestroy {
  private readonly route = inject(ActivatedRoute);
  readonly store = inject(WorkflowUiStore);

  readonly decision = computed(() => this.store.decisionDetail());
  readonly isLoading = computed(() => this.store.decisionDetailLoading());
  readonly errorMessage = computed(() => this.store.decisionDetailError());

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = Number(params.get('id'));

      if (!id || Number.isNaN(id)) {
        this.store.clearDecisionDetail();
        return;
      }

      void this.store.loadDecisionDetail(id);
    });
  }

  ngOnDestroy(): void {
    this.store.clearDecisionDetail();
  }
}