-- just a list of helpful queries I use during testing
select mt.key,us.* from maintoken as mt join usage as us on us.maintoken
id = mt.id;

select seq from sqlite_sequence where name='Usage';

select * from usage LIMIT 2;

-- similar to select @@IDENTITY or SELECT SCOPE_IDENTITY() 
SELECT last_insert_rowid();
