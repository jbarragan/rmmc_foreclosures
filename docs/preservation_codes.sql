create table preservation_code_allowances(
	id int IDENTITY(1,1) PRIMARY KEY,
	preservation_code_id int,
	allowance decimal(14,2),
	effective_date datetime
);

create index pca_preservation_code_id_idx on preservation_code_allowances(preservation_code_id);

insert into preservation_code_allowances(preservation_code_id, allowance, effective_date)
select id, allowance, '2014-01-01' from preservation_codes;