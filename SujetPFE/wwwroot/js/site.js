$(document).ready(function () {
    $('#objectivation-link').click(function () {
        $('#crud-links').show();
        $('#crud-links').html(`
            <a href="/Directions">Directions</a>
            <a href="/Employees">Employees</a>
            <a href="/HistoriqueObjectifs">Historique Objectifs</a>
            <a href="/Clients">Clients</a>
        `);
    });
});