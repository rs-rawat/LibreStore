-- just a list of helpful queries I use during testing
select mt.key,us.* from maintoken as mt join usage as us on us.maintoken
id = mt.id;

