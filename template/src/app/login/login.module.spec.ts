import { LoginModule } from './login.module';
import { ReactiveFormsModule } from '@angular/forms';

describe('LoginModule', () => {
    let loginModule: LoginModule;

    beforeEach(() => {
        loginModule = new LoginModule();
    });

    it('should create an instance', () => {
        expect(loginModule).toBeTruthy();
    });
});
