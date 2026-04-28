import { Routes } from '@angular/router';
import { ApprovalsListComponent } from './features/decisions/approvals/approvals-list.component';
import { DecisionDetailComponent } from './features/decisions/decision-detail.component';
import { DecisionsListComponent } from './features/decisions/decisions-list.component';
import { TraceViewComponent } from './features/decisions/trace/trace-view.component';

export const routes: Routes = [
  { path: '', redirectTo: 'decisions', pathMatch: 'full' },
  { path: 'decisions', component: DecisionsListComponent },
  { path: 'decisions/:id', component: DecisionDetailComponent },
  { path: 'approvals', component: ApprovalsListComponent },
  { path: 'trace/:correlationId', component: TraceViewComponent }
];