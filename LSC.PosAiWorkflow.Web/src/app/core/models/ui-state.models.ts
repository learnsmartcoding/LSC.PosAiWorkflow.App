export interface UiState<T> {
  isLoading: boolean;
  errorMessage: string;
  data: T;
}