document.addEventListener('DOMContentLoaded', function () {
    // Animation d'apparition du formulaire
    const formArea = document.querySelector('.login-box');
    if (formArea) {
        formArea.style.transform = 'translateY(-20px)';
        formArea.style.opacity = 0;
        setTimeout(() => {
            formArea.style.transition = 'transform 0.5s ease-out, opacity 0.5s ease-out';
            formArea.style.transform = 'translateY(0)';
            formArea.style.opacity = 1;
        }, 200);
    }

    // Effet de focus amélioré pour les champs de saisie
    const formControls = document.querySelectorAll('.form-control');
    formControls.forEach(control => {
        control.addEventListener('focus', function () {
            this.parentElement.classList.add('focused');
        });
        control.addEventListener('blur', function () {
            this.parentElement.classList.remove('focused');
        });
    });

    // Effet de vague sur le bouton de connexion
    const loginButton = document.querySelector('.btn-primary');
    if (loginButton) {
        loginButton.addEventListener('click', function (e) {
            const rect = this.getBoundingClientRect();
            const x = e.clientX - rect.left;
            const y = e.clientY - rect.top;

            const ripple = document.createElement('span');
            ripple.classList.add('ripple');
            ripple.style.left = `${x}px`;
            ripple.style.top = `${y}px`;
            this.appendChild(ripple);

            setTimeout(() => {
                ripple.remove();
            }, 600);
        });
    }

    // Validation des champs (optionnel)
    const matriculeInput = document.querySelector('input[name="Matricule"]');
    const passwordInput = document.querySelector('input[name="MotDePasse"]');

    if (matriculeInput && passwordInput) {
        matriculeInput.addEventListener('input', function () {
            if (this.value.length < 5) {
                this.setCustomValidity('La matricule doit contenir au moins 5 caractères.');
            } else {
                this.setCustomValidity('');
            }
        });

        passwordInput.addEventListener('input', function () {
            if (this.value.length < 8) {
                this.setCustomValidity('Le mot de passe doit contenir au moins 8 caractères.');
            } else {
                this.setCustomValidity('');
            }
        });
    }
});