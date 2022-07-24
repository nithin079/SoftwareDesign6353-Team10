import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiResponseModel } from '@app/_models';
import { AccountService, AlertService, FuelQuoteService } from '@app/_services';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-fuel-quote',
  templateUrl: './fuel-quote.component.html',
  styleUrls: ['./fuel-quote.component.less']
})
export class FuelQuoteComponent implements OnInit {
  form: FormGroup;
  loading = false;
  submitted = false;
  deleting = false;
  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private alertService: AlertService,
    private fuelQuoteService:FuelQuoteService,
    private accountService: AccountService,
  ) { }
  account = this.accountService.accountValue;
  ngOnInit(): void {
    this.form = this.formBuilder.group({
      ClientId: [this.account.Id, Validators.required],
      GallonsRequested: ['', Validators.required],
      DiliveryAddress: ['', Validators.required],
      DeliveryDate: ['', Validators.required],
      SuggestedPrice: ['', [Validators.required]],
      TotalAmountDue: ['', [Validators.required]],
  });
  }

   // convenience getter for easy access to form fields
   get f() { return this.form.controls; }

   onSubmit() {
    this.submitted = true;

    // // reset alerts on submit
    this.alertService.clear();

    // // stop here if form is invalid
    if (this.form.invalid) {
        return;
    }

    this.loading = true;
    this.fuelQuoteService.Add(this.form.value)
        .pipe(first())
        .subscribe({
            next: (result:ApiResponseModel) => {
              this.loading = false;
                this.alertService.success('Save successful', { keepAfterRouteChange: true });
               this.router.navigate(['/admin/fuel-history'], { relativeTo: this.route });
            },
            error: error => {
                this.alertService.error(error);
                this.loading = false;
            }
        });
}
}
