document.addEventListener('DOMContentLoaded', function () {
    // Gestion de la barre de navigation latérale
    const navItems = document.querySelectorAll('.sidebar a');

    navItems.forEach(item => {
        item.addEventListener('click', function () {
            navItems.forEach(navItem => navItem.classList.remove('active'));
            this.classList.add('active');
        });
    });

    // Coloration des valeurs KPI
    const kpiValues = document.querySelectorAll('.kpi-value');

    kpiValues.forEach(value => {
        const text = value.textContent;
        if (text.startsWith('+')) {
            value.classList.add('positive'); // Ajouter une classe pour le style positif
        } else if (text.startsWith('-')) {
            value.classList.add('negative'); // Ajouter une classe pour le style négatif
        }
    });

    // Gestion du menu déroulant Objectivation (si nécessaire)
    const objectivationDropdown = document.querySelector('#objectivationDropdown');
    const objectivationMenu = document.querySelector('.objectivation-menu');

    if (objectivationDropdown && objectivationMenu) {
        objectivationDropdown.addEventListener('click', function (event) {
            event.preventDefault();
            objectivationDropdown.parentElement.classList.toggle('show');
            objectivationMenu.classList.toggle('show');
        });
    }

    // Gestion du clic sur le bouton de déconnexion (si nécessaire)
    const logoutLink = document.querySelector('a[href="/Home/Logout"]');
    if (logoutLink) {
        logoutLink.addEventListener('click', function (event) {
            if (!confirm('Êtes-vous sûr de vouloir vous déconnecter ?')) {
                event.preventDefault(); // Empêche le comportement par défaut du lien
            }
        });
    }
});