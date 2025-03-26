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
            value.style.color = 'green';
        } else if (text.startsWith('-')) {
            value.style.color = 'red';
        }
    });
});