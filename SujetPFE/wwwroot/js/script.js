document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('loginForm').addEventListener('submit', function (e) {
        e.preventDefault();

        const matricule = document.getElementById('matricule').value;
        const password = document.getElementById('password').value;
        const errorMessage = document.getElementById('errorMessage');

        errorMessage.textContent = ''; // Clear previous errors

        if (!matricule || !password) {
            errorMessage.textContent = 'Please fill in all fields.';
            return;
        }

        document.querySelector('.login-button').textContent = 'Loading...';

        // Simulate login request (replace with your actual login logic)
        setTimeout(function () {
            if (matricule === 'test' && password === 'password') {
                errorMessage.textContent = 'Login successful!'; // Replace with actual success action
                errorMessage.style.color = "green";
            } else {
                errorMessage.textContent = 'Invalid credentials.';
                errorMessage.style.color = "red";
            }
            document.querySelector('.login-button').textContent = 'Se connecter';
        }, 1000);
    });
});